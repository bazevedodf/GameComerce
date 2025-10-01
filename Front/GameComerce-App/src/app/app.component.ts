import { Component, OnInit } from '@angular/core';
import { SiteInfo } from './model/siteInfo';
import { SiteInfoService } from './services/SiteInfo.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{

  siteInfo?: SiteInfo;
  title = 'GameComerce-App';

  constructor(private siteInfoService: SiteInfoService) {}

  ngOnInit(): void {
    this.siteInfoService.getSiteInfo().subscribe(data => {
      this.siteInfo = data;
    });
  }
}
