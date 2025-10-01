import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {

  @Input() siteInfo: any;
  
  currentYear: number = new Date().getFullYear();

  constructor() { }

  // Método para inscrição na newsletter
  subscribeNewsletter(email: string): void {
    console.log('Inscrição na newsletter:', email);
    // Aqui você implementaria a lógica real
  }
}
