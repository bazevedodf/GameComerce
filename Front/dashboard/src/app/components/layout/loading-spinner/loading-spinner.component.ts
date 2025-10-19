import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.scss']
})
export class LoadingSpinnerComponent {

  @Input() isLoading: boolean = false;
  @Input() message: string = 'Carregando...';
  @Input() subMessage: string = '';
  @Input() zIndex: number = 9999;
  @Input() showProgress: boolean = false;
  @Input() progress: number = 0;

  // Método para mostrar o spinner
  show(message?: string, subMessage?: string): void {
    this.isLoading = true;
    if (message) this.message = message;
    if (subMessage) this.subMessage = subMessage;
  }

  // Método para esconder o spinner
  hide(): void {
    this.isLoading = false;
    this.message = 'Carregando...';
    this.subMessage = '';
    this.progress = 0;
  }

  // Método para atualizar progresso
  updateProgress(progress: number, message?: string): void {
    this.progress = progress;
    if (message) this.message = message;
    this.showProgress = true;
  }
}
