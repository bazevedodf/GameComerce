import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SiteDetalheComponent } from './site-detalhe.component';

describe('SiteDetalheComponent', () => {
  let component: SiteDetalheComponent;
  let fixture: ComponentFixture<SiteDetalheComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SiteDetalheComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SiteDetalheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
