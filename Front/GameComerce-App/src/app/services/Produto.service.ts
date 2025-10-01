import { Injectable } from '@angular/core';
import { Produto } from '../model/Produto';

@Injectable({
  providedIn: 'root',
})
export class ProdutoService {
  private produtos: Produto[] = [
    {
      id: 1,
      nome: '10.000 V-Bucks Fortnite',
      descricao:
        'Pacote de 10.000 V-Bucks para Fortnite. Moeda virtual oficial do jogo.',
      preco: 349.9,
      precoOriginal: 399.9,
      desconto: 13,
      imagem:
        'https://images.unsplash.com/photo-1550745165-9bc0b252726f?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'V-Bucks',
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
      nome: '5.000 Diamantes Free Fire',
      descricao:
        'Diamantes para Free Fire. Compre itens exclusivos na loja do jogo.',
      preco: 149.9,
      precoOriginal: 179.9,
      desconto: 17,
      imagem:
        'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'Diamantes',
      avaliacao: 4.7,
      totalAvaliacoes: 892,
      tags: ['freefire', 'diamantes', 'moeda'],
      ativo: true,
      emDestaque: true,
      plataforma: 'Free Fire',
      entrega: 'Instantâneo',
    },
    {
      id: 3,
      nome: '2.100 Valorant Points',
      descricao:
        'Points para Valorant. Desbloqueie skins e conteúdos exclusivos.',
      preco: 99.9,
      precoOriginal: 119.9,
      desconto: 17,
      imagem:
        'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'Points',
      avaliacao: 4.9,
      totalAvaliacoes: 756,
      tags: ['valorant', 'points', 'moeda'],
      ativo: true,
      emDestaque: false,
      plataforma: 'Valorant',
      entrega: 'Instantâneo',
    },
    {
      id: 4,
      nome: 'Bundle Mestre Fortnite',
      descricao:
        'Pacote especial com skin rara, pickaxe e backbling exclusivos.',
      preco: 79.9,
      precoOriginal: 149.9,
      desconto: 47,
      imagem:
        'https://images.unsplash.com/photo-1552820728-8b83bb6b773f?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'Bundles',
      avaliacao: 4.6,
      totalAvaliacoes: 543,
      tags: ['fortnite', 'bundle', 'skin'],
      ativo: true,
      emDestaque: true,
      plataforma: 'Fortnite',
      entrega: 'Instantâneo',
    },
    {
      id: 5,
      nome: '1.380 RP League of Legends',
      descricao: 'Riot Points para League of Legends. Compre campeões e skins.',
      preco: 49.9,
      precoOriginal: 59.9,
      desconto: 17,
      imagem:
        'https://images.unsplash.com/photo-1534423861386-85a16f5d13fd?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'RP',
      avaliacao: 4.8,
      totalAvaliacoes: 1123,
      tags: ['lol', 'rp', 'moeda'],
      ativo: true,
      emDestaque: false,
      plataforma: 'League of Legends',
      entrega: 'Instantâneo',
    },
    {
      id: 6,
      nome: '4.000 Robux Roblox',
      descricao:
        'Robux para Roblox. A moeda oficial para comprar itens e experiências.',
      preco: 159.9,
      precoOriginal: 189.9,
      desconto: 16,
      imagem:
        'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'Robux',
      avaliacao: 4.5,
      totalAvaliacoes: 678,
      tags: ['roblox', 'robux', 'moeda'],
      ativo: true,
      emDestaque: true,
      plataforma: 'Roblox',
      entrega: 'Instantâneo',
    },
    {
      id: 7,
      nome: '800 Gemas Brawl Stars',
      descricao:
        'Gemas para Brawl Stars. Desbloqueie brawlers e skins especiais.',
      preco: 39.9,
      precoOriginal: 49.9,
      desconto: 20,
      imagem:
        'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'Gemas',
      avaliacao: 4.4,
      totalAvaliacoes: 432,
      tags: ['brawlstars', 'gemas', 'moeda'],
      ativo: false,
      emDestaque: false,
      plataforma: 'Brawl Stars',
      entrega: '24 horas',
    },
    {
      id: 8,
      nome: 'Elite Pass Free Fire',
      descricao: 'Passe de elite com recompensas exclusivas e itens raros.',
      preco: 29.9,
      precoOriginal: 39.9,
      desconto: 25,
      imagem:
        'https://images.unsplash.com/photo-1542751371-adc38448a05e?ixlib=rb-4.0.3&auto=format&fit=crop&w=500&q=80',
      categoria: 'Passe',
      avaliacao: 4.7,
      totalAvaliacoes: 321,
      tags: ['freefire', 'passe', 'elite'],
      ativo: true,
      emDestaque: true,
      plataforma: 'Free Fire',
      entrega: 'Instantâneo',
    },
  ];

  constructor() {}

  // Buscar todos os produtos
  getProdutos(): Produto[] {
    return this.produtos;
  }

  // Buscar produtos em destaque
  getProdutosDestaque(): Produto[] {
    return this.produtos.filter((produto) => produto.emDestaque);
  }

  // Buscar produtos por categoria
  getProdutosPorCategoria(categoria: string): Produto[] {
    return this.produtos.filter(
      (produto) => produto.categoria.toLowerCase() === categoria.toLowerCase()
    );
  }

  // Buscar produtos por plataforma
  getProdutosPorPlataforma(plataforma: string): Produto[] {
    return this.produtos.filter(
      (produto) => produto.plataforma.toLowerCase() === plataforma.toLowerCase()
    );
  }

  // Buscar produto por ID
  getProdutoPorId(id: number): Produto | undefined {
    return this.produtos.find((produto) => produto.id === id);
  }

  // Buscar produtos com desconto
  getProdutosComDesconto(): Produto[] {
    return this.produtos.filter((produto) => produto.desconto > 0);
  }

  // Buscar produtos em estoque (agora usando 'ativo')
  getProdutosEmEstoque(): Produto[] {
    return this.produtos.filter((produto) => produto.ativo);
  }
}
