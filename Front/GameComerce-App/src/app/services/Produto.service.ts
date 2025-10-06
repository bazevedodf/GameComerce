import { Injectable } from '@angular/core';
import { Produto } from '../model/Produto';
import { HttpClient } from '@angular/common/http';
import { Observable, of, tap } from 'rxjs';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProdutoService {
  
  private useMock = false; // Mude para false quando API estiver pronta
  private produtosCache: Produto[] = [];
  private produtosDestaqueCache: Produto[] = [];
  private apiUrl = environment.apiUrl+'Produtos';
  private imgUrl = environment.imgUrl;

  constructor(private http: HttpClient) {

  }

  private carregarProdutosIniciais(): void {
    if (this.useMock) {
      // Dados mockados iniciais
      this.produtosCache = [
        {
          id: 1,
          nome: '13.500 V-Bucks Fortnite',
          descricao: 'Pacote de 10.000 V-Bucks para Fortnite. Moeda virtual oficial do jogo.',
          preco: 127.90,
          precoOriginal: 313.99,
          desconto: 13,
          imagem: '../../assets/img/fortnite-13500vb.png',
          categoria: { 
            id: 1,
            name: "FORTNITE", 
            hasDropdown: true,
            slug: "fortnite",
            descricao: "Explore, crie e brilhe no Fortnite com estilo!",
            imagem: "../../assets/img/categoria-fortnite.png",
            icon: "assets/img/fortnite-ico.png"
          },
          avaliacao: 4.8,
          totalAvaliacoes: 1247,
          tags: ['fortnite', 'v-bucks', 'moeda'],
          ativo: true,
          emDestaque: true,
          plataforma: 'Fortnite',
          entrega: 'Instantâneo',
        },
        {
          id: 2,
          nome: '5000 V-Bucks Fortnite',
          descricao: 'Pacote de 5000 V-Bucks para Fortnite. Moeda virtual oficial do jogo.',
          preco: 64.90,
          precoOriginal: 243.89,
          desconto: 13,
          imagem: '../../assets/img/fortnite-5000vb.png',
          categoria: { 
            id: 1,
            name: "FORTNITE", 
            hasDropdown: true,
            slug: "fortnite",
            descricao: "Explore, crie e brilhe no Fortnite com estilo!",
            imagem: "../../assets/img/categoria-fortnite.png",
            icon: "assets/img/fortnite-ico.png"
          },
          avaliacao: 3.8,
          totalAvaliacoes: 1048,
          tags: ['fortnite', 'v-bucks', 'moeda'],
          ativo: true,
          emDestaque: true,
          plataforma: 'Fortnite',
          entrega: 'Instantâneo',
        },
        {
          id: 3,
          nome: '5.000 Diamantes Free Fire',
          descricao: 'Diamantes para Free Fire. Compre itens exclusivos na loja do jogo.',
          preco: 149.9,
          precoOriginal: 179.9,
          desconto: 17,
          imagem: 'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
          categoria: { 
            id: 2,
            name: "FREE FIRE", 
            hasDropdown: true,
            slug: "free-fire",
            descricao: "Ação rápida e intensa em batalhas épicas.",
            imagem: "../../assets/img/categoria-free-fire.webp",
            icon: "assets/img/freefire-ico.png"
          },
          avaliacao: 4.7,
          totalAvaliacoes: 892,
          tags: ['freefire', 'diamantes', 'moeda'],
          ativo: true,
          emDestaque: true,
          plataforma: 'Free Fire',
          entrega: 'Instantâneo',
        },
        {
          id: 4,
          nome: '2.100 Valorant Points',
          descricao: 'Points para Valorant. Desbloqueie skins e conteúdos exclusivos.',
          preco: 99.9,
          precoOriginal: 119.9,
          desconto: 17,
          imagem: 'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
          categoria: { 
            id: 3,
            name: "VALORANT", 
            hasDropdown: false,
            slug: "valorant",
            descricao: "Ação tática com estilo e precisão.",
            imagem: "../../assets/img/categoria-valorant.jpg",
            icon: "assets/img/valorant-ico.png"
          },
          avaliacao: 4.9,
          totalAvaliacoes: 756,
          tags: ['valorant', 'points', 'moeda'],
          ativo: true,
          emDestaque: false,
          plataforma: 'Valorant',
          entrega: 'Instantâneo',
        },
        {
          id: 5,
          nome: 'Bundle Mestre Fortnite',
          descricao: 'Pacote especial com skin rara, pickaxe e backbling exclusivos.',
          preco: 79.9,
          precoOriginal: 149.9,
          desconto: 47,
          imagem: 'https://images.unsplash.com/photo-1552820728-8b83bb6b773f?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
          categoria: { 
            id: 1,
            name: "FORTNITE", 
            hasDropdown: true,
            slug: "fortnite",
            descricao: "Explore, crie e brilhe no Fortnite com estilo!",
            imagem: "../../assets/img/categoria-fortnite.png",
            icon: "assets/img/fortnite-ico.png"
          },
          avaliacao: 4.6,
          totalAvaliacoes: 543,
          tags: ['fortnite', 'bundle', 'skin'],
          ativo: true,
          emDestaque: true,
          plataforma: 'Fortnite',
          entrega: 'Instantâneo',
        },
        {
          id: 6,
          nome: '1.380 RP League of Legends',
          descricao: 'Riot Points para League of Legends. Compre campeões e skins.',
          preco: 49.9,
          precoOriginal: 59.9,
          desconto: 17,
          imagem: 'https://images.unsplash.com/photo-1534423861386-85a16f5d13fd?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
          categoria: { 
            id: 4,
            name: "LEAGUE OF LEGENDS", 
            hasDropdown: false,
            slug: "league-of-legends",
            descricao: "Batalhas estratégicas em um mundo de fantasia.",
            imagem: "../../assets/img/categoria-legue-of-legend.png",
            icon: "assets/img/legue-of-legends-ico.png"
          },
          avaliacao: 4.8,
          totalAvaliacoes: 1123,
          tags: ['lol', 'rp', 'moeda'],
          ativo: true,
          emDestaque: false,
          plataforma: 'League of Legends',
          entrega: 'Instantâneo',
        },
        {
          id: 7,
          nome: '4.000 Robux Roblox',
          descricao: 'Robux para Roblox. A moeda oficial para comprar itens e experiências.',
          preco: 159.9,
          precoOriginal: 189.9,
          desconto: 16,
          imagem: 'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
          categoria: { 
            id: 5,
            name: "ROBLOX", 
            hasDropdown: false,
            slug: "roblox",
            descricao: "Explore, crie e brilhe no Roblox com estilo!",
            imagem: "../../assets/img/categoria-roblox.webp",
            icon: "assets/img/robux-ico.png"
          },
          avaliacao: 4.5,
          totalAvaliacoes: 678,
          tags: ['roblox', 'robux', 'moeda'],
          ativo: true,
          emDestaque: true,
          plataforma: 'Roblox',
          entrega: 'Instantâneo',
        },
        {
          id: 8,
          nome: '800 Gemas Brawl Stars',
          descricao: 'Gemas para Brawl Stars. Desbloqueie brawlers e skins especiais.',
          preco: 39.9,
          precoOriginal: 49.9,
          desconto: 20,
          imagem: 'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
          categoria: { 
            id: 6,
            name: "BRAWL STARS", 
            hasDropdown: false,
            slug: "brawl-stars",
            descricao: "Lutas rápidas e divertidas com amigos.",
            imagem: "../../assets/img/categoria-brawl-stars.webp",
            icon: "assets/img/brawstars-ico.png"
          },
          avaliacao: 4.4,
          totalAvaliacoes: 432,
          tags: ['brawlstars', 'gemas', 'moeda'],
          ativo: false,
          emDestaque: false,
          plataforma: 'Brawl Stars',
          entrega: '24 horas',
        },
        {
          id: 9,
          nome: 'Elite Pass Free Fire',
          descricao: 'Passe de elite com recompensas exclusivas e itens raros.',
          preco: 29.9,
          precoOriginal: 39.9,
          desconto: 25,
          imagem: 'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
          categoria: { 
            id: 2,
            name: "FREE FIRE", 
            hasDropdown: true,
            slug: "free-fire",
            descricao: "Ação rápida e intensa em batalhas épicas.",
            imagem: "../../assets/img/categoria-free-fire.webp",
            icon: "assets/img/freefire-ico.png"
          },
          avaliacao: 4.7,
          totalAvaliacoes: 321,
          tags: ['freefire', 'passe', 'elite'],
          ativo: true,
          emDestaque: true,
          plataforma: 'Free Fire',
          entrega: 'Instantâneo',
        }
      ];
    }
  }

  // MÉTODO PARA BUSCAR TODOS OS PRODUTOS (API ou Mock)
  getProdutos(): Observable<Produto[]> {
    if (this.useMock) {
      return of(this.produtosCache);
    } else {
      return this.http.get<Produto[]>(this.apiUrl).pipe(
        tap(produtos => {
          this.produtosCache = produtos; // Atualiza cache com dados da API
        })
      );
    }
  }

  // MÉTODO PARA BUSCAR PRODUTO POR ID
  getProdutoPorId(id: number): Observable<Produto | undefined> {
    if (this.useMock) {
      const produto = this.produtosCache.find(p => p.id === id);
      return of(produto);
    } else {
      return this.http.get<Produto>(`${this.apiUrl}/produtos/${id}`);
    }
  }

  // MÉTODO PARA BUSCAR PRODUTOS POR CATEGORIA
  getProdutosPorCategoria(categoriaSlug: string): Observable<Produto[]> {
    if (this.useMock) {
      const produtos = this.produtosCache.filter(p => p.categoria.slug === categoriaSlug);
      return of(produtos);
    } else {
      return this.http.get<Produto[]>(`${this.apiUrl}/produtos?categoria=${categoriaSlug}`);
    }
  }

  // MÉTODO PARA BUSCAR PRODUTOS POR TERMO DE BUSCA
  buscarProdutos(termo: string): Observable<Produto[]> {
    if (this.useMock) {
      const produtos = this.produtosCache.filter(p => 
        p.nome.toLowerCase().includes(termo.toLowerCase()) ||
        p.descricao.toLowerCase().includes(termo.toLowerCase()) ||
        p.categoria.name.toLowerCase().includes(termo.toLowerCase())
      );
      return of(produtos);
    } else {
      return this.http.get<Produto[]>(`${this.apiUrl}/produtos/busca?q=${termo}`);
    }
  }

  // MÉTODO PARA ATUALIZAR CACHE QUANDO API ESTIVER PRONTA
  setUseMock(usarMock: boolean): void {
    this.useMock = usarMock;
    if (!usarMock) {
      // Limpa caches quando mudar para API real
      this.produtosCache = [];
      this.produtosDestaqueCache = [];
    }
  }

  // MÉTODO PARA ADICIONAR PRODUTO (admin)
  adicionarProduto(produto: Produto): Observable<Produto> {
    if (this.useMock) {
      // Adiciona ao mock
      produto.id = Math.max(...this.produtosCache.map(p => p.id)) + 1;
      this.produtosCache.push(produto);
      return of(produto);
    } else {
      return this.http.post<Produto>(`${this.apiUrl}/produtos`, produto);
    }
  }
  


}
