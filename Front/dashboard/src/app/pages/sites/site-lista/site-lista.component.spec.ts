import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SiteListaComponent } from './site-lista.component';

describe('SiteListaComponent', () => {
  let component: SiteListaComponent;
  let fixture: ComponentFixture<SiteListaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SiteListaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SiteListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
