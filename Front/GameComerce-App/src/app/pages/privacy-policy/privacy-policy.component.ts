import { Component, OnInit } from '@angular/core';
import { SiteInfo } from 'src/app/model/siteInfo';
import { SiteInfoService } from 'src/app/services/SiteInfo.service';

@Component({
  selector: 'app-privacy-policy',
  templateUrl: './privacy-policy.component.html',
  styleUrls: ['./privacy-policy.component.scss']
})
export class PrivacyPolicyComponent implements OnInit {

  siteInfo?: SiteInfo;
  currentDate: Date = new Date();

  constructor(private siteInfoService: SiteInfoService) { }

  ngOnInit(): void {
    this.siteInfoService.getSiteInfo().subscribe(data => {
      this.siteInfo = data;
    });
  }

}