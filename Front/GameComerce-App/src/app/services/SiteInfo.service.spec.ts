/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SiteInfoService } from './SiteInfo.service';

describe('Service: SiteInfo', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SiteInfoService]
    });
  });

  it('should ...', inject([SiteInfoService], (service: SiteInfoService) => {
    expect(service).toBeTruthy();
  }));
});
