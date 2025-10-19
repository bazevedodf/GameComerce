import { Component, Input } from '@angular/core';

export type ToastType = 'success' | 'error' | 'warning' | 'info';

@Component({
  selector: 'app-toast-message',
  templateUrl: './toast-message.component.html',
  styleUrls: ['./toast-message.component.scss']
})
export class ToastMessageComponent {
  @Input() message: string = '';
  @Input() type: ToastType = 'info';
  @Input() seconds: number = 5;
  isVisible: boolean = false;
  isHiding: boolean = false; // para controlar animação de saída

  show() {
    this.isVisible = true;
    this.isHiding = false;
    
    setTimeout(() => {
      this.hide();
    }, this.seconds * 1000);
  }

  hide() {
    this.isHiding = true; // animação de saída
    
    // Espera a animação terminar antes de esconder completamente
    setTimeout(() => {
      this.isVisible = false;
      this.isHiding = false;
    }, 300); //Tempo da animação slideOut
  }

  getIconClass(): string {
    const iconClasses = {
      'success': 'bi bi-check-circle-fill text-success',
      'error': 'bi bi-exclamation-circle-fill text-danger', 
      'warning': 'bi bi-exclamation-triangle-fill text-warning',
      'info': 'bi bi-info-circle-fill text-info'
    };
    return iconClasses[this.type];
  }
}