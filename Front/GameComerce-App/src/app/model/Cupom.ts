export interface Cupom {
  id: number;
  codigo: string;
  valido: boolean;
  valorDesconto?: number;
  tipoDesconto: 'percentual' | 'valor_fixo';
  mensagemErro?: string;
}