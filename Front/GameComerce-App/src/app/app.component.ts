import { Component, OnInit, Renderer2 } from '@angular/core';
import { SiteInfo } from './model/siteInfo';
import { SiteInfoService } from './services/SiteInfo.service';
import { MarketingTagService } from './services/marketingTag.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{

  siteInfo?: SiteInfo;
  title = 'GameComerce-App';

  constructor(private siteInfoService: SiteInfoService, 
              private marketingTagService: MarketingTagService
            ) {}

  ngOnInit(): void {
    this.siteInfoService.getSiteInfo().subscribe(data => {
      this.siteInfo = data;

      // Carrega as tags de marketing se existirem
      if (data.marketingTags && data.marketingTags.length > 0) {
        this.marketingTagService.carregarTags(data.marketingTags);
      }
    });
  }

}
