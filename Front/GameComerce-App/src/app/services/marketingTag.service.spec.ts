/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { MarketingTagService } from './marketingTag.service';

describe('Service: MarketingTag', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MarketingTagService]
    });
  });

  it('should ...', inject([MarketingTagService], (service: MarketingTagService) => {
    expect(service).toBeTruthy();
  }));
});
