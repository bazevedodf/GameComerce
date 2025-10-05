import { Component, OnInit } from '@angular/core';
import { SiteInfo } from 'src/app/model/siteInfo';
import { SiteInfoService } from 'src/app/services/SiteInfo.service';

@Component({
  selector: 'app-terms-of-use',
  templateUrl: './terms-of-use.component.html',
  styleUrls: ['./terms-of-use.component.scss']
})
export class TermsOfUseComponent implements OnInit {

  siteInfo?: SiteInfo;
  currentDate: Date = new Date();

  constructor(private siteInfoService: SiteInfoService) { }

  ngOnInit(): void {
    this.siteInfoService.getSiteInfo().subscribe(data => {
      this.siteInfo = data;
    });
  }

  getCityFromAddress(): string {
    if (!this.siteInfo?.address) {
      return 'São Paulo';
    }
    
    try {
      // Tenta extrair a cidade do endereço (assumindo formato "Rua, Número - Cidade, Estado")
      const parts = this.siteInfo.address.split('-');
      if (parts.length > 1) {
        const cityPart = parts[1].split(',')[0]?.trim();
        return cityPart || 'São Paulo';
      }
      return 'São Paulo';
    } catch (error) {
      return 'São Paulo';
    }
  }

}