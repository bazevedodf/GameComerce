import { Injectable, Renderer2, RendererFactory2, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MarketingTag } from '../model/MarketingTag';
import { Produto } from '@app/model/Produto';
import { CartItem } from '@app/model/CartIem';

declare global {
  interface Window {
    dataLayer: any[];
    fbq?: any;
    _fbq?: any;
    gtm_loaded?: boolean;
    fb_pixel_loaded?: boolean;
    tt_pixel_loaded?: boolean;
    ga_loaded?: boolean;
    [key: string]: any;
  }
}

@Injectable({
  providedIn: 'root'
})
export class MarketingTagService {

  private renderer: Renderer2;
  private utmParameters: { [key: string]: string } = {};
  private tagsCarregadas: { [key: string]: boolean } = {
    'facebook-pixel': false,
    'google-analytics': false, 
    'tiktok-pixel': false,
    'google-tag-manager': false
  };

  constructor(
    private route: ActivatedRoute,
    private rendererFactory: RendererFactory2,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.renderer = this.rendererFactory.createRenderer(null, null);
    this.capturarUtmParameters();
  }

  // ========== MÉTODOS PÚBLICOS ==========
  private capturarUtmParameters(): void {
    this.route.queryParams.subscribe(params => {
      // Salva todos os UTM parameters
      this.utmParameters = {
        utm_source: params['utm_source'] || params['source'],
        utm_medium: params['utm_medium'] || params['medium'],
        utm_campaign: params['utm_campaign'] || params['campaign'],
        utm_term: params['utm_term'] || params['term'],
        utm_content: params['utm_content'] || params['content']
      };
    });
  }

  carregarTags(tags: MarketingTag[]): void {
    const utmSource = this.utmParameters['utm_source'];
    
    if (!utmSource) {
      return;
    }

    const tag = tags.find(t => t.identificador.toLowerCase() === utmSource.toLowerCase());
    const tagGoogleAnalitcs = tags.find(t => t.tipo.toLowerCase() === "google-analytics");

    if (tagGoogleAnalitcs) {
      this.carregarTag(tagGoogleAnalitcs);
    }

    if (tag) {
      this.carregarTag(tag);
    }
  }

  dispararAddToCart(produto: Produto, quantidade: number = 1): void {
    try {

      if (this.isFacebookPixelAvailable()) {
        window['fbq']('track', 'AddToCart', {
          content_ids: [produto.id.toString()],
          content_name: produto.nome,
          content_category: produto.categoria?.name || 'Games',
          content_type: 'product',
          value: produto.preco,
          currency: 'BRL',
          quantity: quantidade
        });
      }

      if (this.isGoogleAnalyticsAvailable()) {
        window['gtag']('event', 'add_to_cart', {
          currency: 'BRL',
          value: produto.preco * quantidade,
          items: [{
            item_id: produto.id.toString(),
            item_name: produto.nome,
            item_category: produto.categoria?.name || 'Games',
            price: produto.preco,
            quantity: quantidade
          }]
        });
      }

      if (this.isTiktokPixelAvailable()) {
        window['ttq'].track('AddToCart', {
          contents: [{
            content_id: produto.id.toString(),
            content_name: produto.nome,
            content_category: produto.categoria?.name || 'Games',
            price: produto.preco,
            quantity: quantidade
          }],
          value: produto.preco * quantidade,
          currency: 'BRL'
        });
      }

    } catch (error) {
    }
  }

  dispararInitiateCheckout(carrinho: { itens: CartItem[], total: number, subtotal: number, frete: number }): void {
    try {

      const contents = carrinho.itens.map(item => ({
        content_id: item.produto.id.toString(),
        content_name: item.produto.nome,
        content_category: item.produto.categoria?.name || 'Games',
        quantity: item.quantidade,
        price: item.produto.preco
      }));

      if (this.isFacebookPixelAvailable()) {
        window['fbq']('track', 'InitiateCheckout', {
          contents: contents,
          content_type: 'product',
          value: carrinho.total,
          currency: 'BRL',
          num_items: carrinho.itens.reduce((total, item) => total + item.quantidade, 0)
        });
      }

      if (this.isGoogleAnalyticsAvailable()) {
        window['gtag']('event', 'begin_checkout', {
          currency: 'BRL',
          value: carrinho.total,
          items: carrinho.itens.map(item => ({
            item_id: item.produto.id.toString(),
            item_name: item.produto.nome,
            item_category: item.produto.categoria?.name || 'Games',
            price: item.produto.preco,
            quantity: item.quantidade
          }))
        });
      }

      if (this.isTiktokPixelAvailable()) {
        window['ttq'].track('InitiateCheckout', {
          contents: contents,
          value: carrinho.total,
          currency: 'BRL'
        });
      }

    } catch (error) {
    }
  }

  dispararPurchase(pedido: { id: string, total: number, frete: number, itens: CartItem[] }): void {
    try {

      const contents = pedido.itens.map(item => ({
        content_id: item.produto.id.toString(),
        content_name: item.produto.nome,
        content_category: item.produto.categoria?.name || 'Games',
        quantity: item.quantidade,
        price: item.produto.preco
      }));

      if (this.isFacebookPixelAvailable()) {
        window['fbq']('track', 'Purchase', {
          contents: contents,
          content_type: 'product',
          value: pedido.total,
          currency: 'BRL',
          num_items: pedido.itens.reduce((total, item) => total + item.quantidade, 0)
        });
      }

      if (this.isGoogleAnalyticsAvailable()) {
        window['gtag']('event', 'purchase', {
          transaction_id: pedido.id,
          value: pedido.total,
          currency: 'BRL',
          shipping: pedido.frete,
          items: pedido.itens.map(item => ({
            item_id: item.produto.id.toString(),
            item_name: item.produto.nome,
            item_category: item.produto.categoria?.name || 'Games',
            price: item.produto.preco,
            quantity: item.quantidade
          }))
        });
      }

      if (this.isTiktokPixelAvailable()) {
        window['ttq'].track('CompletePayment', {
          contents: contents,
          value: pedido.total,
          currency: 'BRL'
        });
      }

    } catch (error) {
    }
  }

  // ========== MÉTODOS PRIVADOS - CARREGAMENTO DE TAGS ==========

  private carregarTag(tag: MarketingTag): void {
    switch (tag.tipo) {
      case 'google-tag-manager':
        this.carregarGoogleTagManager(tag.tagId);
        break;
      case 'facebook-pixel':
        this.carregarFacebookPixel(tag.tagId);
        break;
      case 'tiktok-pixel':
        this.carregarTiktokPixel(tag.tagId);
        break;
      case 'google-analytics':
        this.carregarGoogleAnalytics(tag.tagId);
        break;
    }
  }

  private carregarGoogleTagManager(gtmId: string): void {
    if (this.isGtmAlreadyLoaded()) {
      this.registrarTagCarregada('google-tag-manager');
      return;
    }

    this.initializeDataLayer();

    const head = this.document.head;
    const body = this.document.body;

    const scriptId = `gtm-script-${gtmId}`;
    if (!this.document.getElementById(scriptId)) {
      const script = this.renderer.createElement('script');
      this.renderer.setAttribute(script, 'id', scriptId);
      this.renderer.setAttribute(script, 'async', 'true');
      this.renderer.setAttribute(script, 'src', `https://www.googletagmanager.com/gtag/js?id=${gtmId}`);
      this.renderer.appendChild(head, script);
    }

    const configId = `gtm-config-${gtmId}`;
    if (!this.document.getElementById(configId)) {
      const configScript = this.renderer.createElement('script');
      this.renderer.setAttribute(configScript, 'id', configId);
      this.renderer.setProperty(configScript, 'innerHTML', `
        window.dataLayer = window.dataLayer || [];
        function gtag(){dataLayer.push(arguments);}
        gtag('js', new Date());
        gtag('config', '${gtmId}');
      `);
      this.renderer.appendChild(head, configScript);
    }

    const noscriptId = `gtm-noscript-${gtmId}`;
    if (!this.document.getElementById(noscriptId)) {
      const noscript = this.renderer.createElement('noscript');
      this.renderer.setAttribute(noscript, 'id', noscriptId);
      this.renderer.setProperty(noscript, 'innerHTML', `
        <iframe src="https://www.googletagmanager.com/ns.html?id=${gtmId}"
                height="0" width="0" style="display:none;visibility:hidden">
        </iframe>
      `);
      this.renderer.appendChild(body, noscript);
    }

    window.gtm_loaded = true;
    this.registrarTagCarregada('google-tag-manager');
  }

  private carregarFacebookPixel(pixelId: string): void {
    
    // VERIFICAÇÃO CRÍTICA - Se já existe QUALQUER Pixel Facebook na página
    if (this.isAnyFacebookPixelLoaded()) {
      this.registrarTagCarregada('facebook-pixel'); // Importante: marca como disponível para eventos
      return;
    }

    // Verificação específica do nosso Pixel
    if (this.isFacebookPixelAlreadyLoaded(pixelId)) {
      this.registrarTagCarregada('facebook-pixel');
      return;
    }

    const head = this.document.head;
    const body = this.document.body;

    const scriptId = `fb-pixel-script-${pixelId}`;
    if (!this.document.getElementById(scriptId)) {
      const script = this.renderer.createElement('script');
      this.renderer.setAttribute(script, 'id', scriptId);
      this.renderer.setAttribute(script, 'async', 'true');
      this.renderer.setAttribute(script, 'src', 'https://connect.facebook.net/en_US/fbevents.js');
      this.renderer.appendChild(head, script);
    }

    this.initializeFbq(pixelId);

    const noscriptId = `fb-pixel-noscript-${pixelId}`;
    if (!this.document.getElementById(noscriptId)) {
      const noscript = this.renderer.createElement('noscript');
      this.renderer.setAttribute(noscript, 'id', noscriptId);
      this.renderer.setProperty(noscript, 'innerHTML', `
        <img height="1" width="1" style="display:none"
        src="https://www.facebook.com/tr?id=${pixelId}&ev=PageView&noscript=1"/>
      `);
      this.renderer.appendChild(body, noscript);
    }

    window.fb_pixel_loaded = true;
    this.registrarTagCarregada('facebook-pixel');
  }

  private carregarTiktokPixel(pixelId: string): void {
    if (this.isTiktokPixelAlreadyLoaded(pixelId)) {
      this.registrarTagCarregada('tiktok-pixel');
      return;
    }

    const head = this.document.head;

    const scriptId = `tt-pixel-script-${pixelId}`;
    if (!this.document.getElementById(scriptId)) {
      const script = this.renderer.createElement('script');
      this.renderer.setAttribute(script, 'id', scriptId);
      this.renderer.setProperty(script, 'innerHTML', `
        !function (w, d, t) {
          w.TiktokAnalyticsObject=t;var ttq=w[t]=w[t]||[];ttq.methods=["page","track","identify","instances","debug","on","off","once","ready","alias","group","enableCookie","disableCookie"],ttq.setAndDefer=function(t,e){t[e]=function(){t.push([e].concat(Array.prototype.slice.call(arguments,0)))}};for(var i=0;i<ttq.methods.length;i++)ttq.setAndDefer(ttq,ttq.methods[i]);ttq.instance=function(t){for(var e=ttq._i[t]||[],n=0;n<ttq.methods.length;n++)ttq.setAndDefer(e,ttq.methods[n]);return e},ttq.load=function(e,n){var i="https://analytics.tiktok.com/i18n/pixel/events.js";ttq._i=ttq._i||{},ttq._i[e]=[],ttq._i[e]._u=i,ttq._t=ttq._t||{},ttq._t[e]=+new Date,ttq._o=ttq._o||{},ttq._o[e]=n||{};var o=document.createElement("script");o.type="text/javascript",o.async=!0,o.src=i+"?sdkid="+e+"&lib="+t;var a=document.getElementsByTagName("script")[0];a.parentNode.insertBefore(o,a)};
          ttq.load('${pixelId}');
          ttq.page();
        }(window, document, 'ttq');
      `);
      this.renderer.appendChild(head, script);
    }

    window.tt_pixel_loaded = true;
    this.registrarTagCarregada('tiktok-pixel');
  }

  private carregarGoogleAnalytics(gaId: string): void {
    if (this.isGoogleAnalyticsAlreadyLoaded(gaId)) {
      this.registrarTagCarregada('google-analytics');
      return;
    }

    const head = this.document.head;

    const scriptId = `ga-script-${gaId}`;
    if (!this.document.getElementById(scriptId)) {
      const script = this.renderer.createElement('script');
      this.renderer.setAttribute(script, 'id', scriptId);
      this.renderer.setAttribute(script, 'async', 'true');
      this.renderer.setAttribute(script, 'src', `https://www.googletagmanager.com/gtag/js?id=${gaId}`);
      this.renderer.appendChild(head, script);
    }

    const configId = `ga-config-${gaId}`;
    if (!this.document.getElementById(configId)) {
      const configScript = this.renderer.createElement('script');
      this.renderer.setAttribute(configScript, 'id', configId);
      this.renderer.setProperty(configScript, 'innerHTML', `
        window.dataLayer = window.dataLayer || [];
        function gtag(){dataLayer.push(arguments);}
        gtag('js', new Date());
        gtag('config', '${gaId}');
      `);
      this.renderer.appendChild(head, configScript);
    }

    window.ga_loaded = true;
    this.registrarTagCarregada('google-analytics');
  }

  // ========== MÉTODOS AUXILIARES ==========

  private registrarTagCarregada(tipo: string): void {
    this.tagsCarregadas[tipo] = true;
  }

  private isTagCarregada(tipo: string): boolean {
    return this.tagsCarregadas[tipo] || false;
  }

  private isFacebookPixelAvailable(): boolean {
    return this.isTagCarregada('facebook-pixel') && 
           typeof window['fbq'] === 'function';
  }

  private isGoogleAnalyticsAvailable(): boolean {
    return this.isTagCarregada('google-analytics') && 
           typeof window['gtag'] === 'function';
  }

  private isTiktokPixelAvailable(): boolean {
    return this.isTagCarregada('tiktok-pixel') && 
           typeof window['ttq'] === 'function';
  }

  private isGtmAlreadyLoaded(): boolean {
    return !!window.gtm_loaded ||
           !!this.document.querySelector('script[src*="googletagmanager.com/gtag/js"]');
  }

  private isAnyFacebookPixelLoaded(): boolean {
    // Verifica se já existe QUALQUER Pixel Facebook na página
    return typeof window['fbq'] === 'function' && window['fbq'].loaded === true;
  }

  private isFacebookPixelAlreadyLoaded(pixelId: string): boolean {
    return this.document.getElementById(`fb-pixel-script-${pixelId}`) !== null ||
           this.document.getElementById(`fb-pixel-noscript-${pixelId}`) !== null ||
           window.fb_pixel_loaded === true;
  }

  private isTiktokPixelAlreadyLoaded(pixelId: string): boolean {
    return this.document.getElementById(`tt-pixel-script-${pixelId}`) !== null ||
           window.tt_pixel_loaded === true;
  }

  private isGoogleAnalyticsAlreadyLoaded(gaId: string): boolean {
    return this.document.getElementById(`ga-script-${gaId}`) !== null ||
           this.document.getElementById(`ga-config-${gaId}`) !== null ||
           window.ga_loaded === true;
  }

  private initializeDataLayer(): void {
    if (!window.dataLayer) {
      window.dataLayer = [];
      window.dataLayer.push({
        'gtm.start': new Date().getTime(),
        event: 'gtm.js'
      });
    }
  }

  private initializeFbq(pixelId: string): void {
    if (typeof window['fbq'] !== 'function') {
      window['fbq'] = function() {
        window['fbq'].callMethod ? 
        window['fbq'].callMethod.apply(window['fbq'], arguments) : 
        window['fbq'].queue.push(arguments);
      };
      window['fbq'].push = window['fbq'];
      window['fbq'].loaded = true;
      window['fbq'].version = '2.0';
      window['fbq'].queue = [];
    }

    window['fbq']('init', pixelId);
    window['fbq']('track', 'PageView');
  }
}