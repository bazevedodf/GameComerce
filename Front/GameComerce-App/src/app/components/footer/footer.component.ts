import { Component, Input, OnInit } from '@angular/core';
import { SiteInfoService } from 'src/app/services/SiteInfo.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  @Input() siteInfo: any;
  
  currentYear: number = new Date().getFullYear();

  constructor(private siteInfoService: SiteInfoService) { }

  ngOnInit(): void {
    this.siteInfoService.getSiteInfo().subscribe(data => {
      this.siteInfo = data;
    });
  }

  // Método para inscrição na newsletter
  subscribeNewsletter(email: string): void {
    console.log('Inscrição na newsletter:', email);
    // Aqui você implementaria a lógica real
  }
}
