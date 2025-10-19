import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { LayoutService } from '@app/services/layout.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  @Input() isCollapsed = false;
  @Output() toggleSidebar = new EventEmitter<boolean>();
  
  isMobile = false;
  currentRoute = '';

  constructor(
    private layoutService: LayoutService,
    private router: Router
  ) {}

  ngOnInit() {
    this.layoutService.isMobile$.subscribe(isMobile => {
      this.isMobile = isMobile;
    });

    // Monitorar mudanças de rota
    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd)
      )
      .subscribe((event: NavigationEnd) => {
        this.currentRoute = event.urlAfterRedirects;
      });

    // Definir rota inicial
    this.currentRoute = this.router.url;
  }

  toggle() {
    const newState = !this.isCollapsed;
    this.isCollapsed = newState;
    this.toggleSidebar.emit(newState);
  }

  setActiveItem(event: Event) {
    event.preventDefault();
    
    // Em mobile, fecha o sidebar após clicar em um item
    if (this.isMobile) {
      this.toggle();
    }
  }
}