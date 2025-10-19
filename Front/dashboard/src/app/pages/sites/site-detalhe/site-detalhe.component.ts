import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponent } from '@app/app.component';
import { SiteInfo } from '@app/models/SiteInfo';
import { SiteInfoService } from '@app/services/siteInfo.service';

@Component({
  selector: 'app-site-detalhe',
  templateUrl: './site-detalhe.component.html',
  styleUrls: ['./site-detalhe.component.scss']
})
export class SiteDetalheComponent implements OnInit {

  public form!: FormGroup;
  site: SiteInfo | null = null;
  isEditMode = false;
  loading = false;
  submitted = false;

  get f() { 
    return this.form.controls; 
  }

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private siteService: SiteInfoService,
    private appComponent: AppComponent
  ) {
    
  }

  ngOnInit() {
    this.createForm();
    this.modoEditar();
  }

  public validaCSS(campo: FormControl | AbstractControl): any {
    return { "is-invalid": campo?.errors && campo.touched };
  }

  private modoEditar(){
    const id = this.route.snapshot.paramMap.get('id');
    if(id)
    {
      this.loadSite(Number(id));
      this.isEditMode = true
    }
    else
    {
      this.isEditMode = false;
    }
  }

  createForm(): void {
    this.form = this.fb.group({
      id: [0],
      nome: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      dominio: ['', Validators.required],
      cnpj: ['', [Validators.required, Validators.pattern(/^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$/)]],
      address: ['', [Validators.required, Validators.minLength(5)]],
      email: ['', [Validators.required, Validators.email]],
      instagram: [''],
      facebook: [''],
      whatsapp: ['', [Validators.required]],
      apiKey: [''],
      baseUrl: [''],
      ativo: [true]
    });
  }

  loadSite(id: number): void {  
    this.appComponent.showGlobalLoading('Carregando site...','Aguarde um momento');
    
    this.siteService.getSiteById(id).subscribe({
      next: (site) => {
        this.site = site;
        this.site.id = site.id;
        this.form.patchValue(site);
        this.loading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar site:', error);
        this.loading = false;
        // Redirecionar para lista em caso de erro
        this.router.navigate(['/sites/lista']);
      }
    }).add(() => { this.appComponent.hideGlobalLoading(); });
  }

  onSubmit(): void {
    this.appComponent.showGlobalLoading(
      this.isEditMode ? 'Atualizando site...' : 'Criando site...',
      'Aguarde um momento'
    );

    console.log('Form valid:', this.form.valid);
    console.log('Form errors:', this.form.errors);
    console.log('Form values:', this.form.value);
    
    // DEBUG: Mostrar erros de cada campo
    Object.keys(this.form.controls).forEach(key => {
      const control = this.form.get(key);
      if (control?.invalid) {
        console.log(`Campo ${key} inválido:`, control.errors);
      }
    });

    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    const formData = this.form.value;

    if (this.isEditMode) {
      this.siteService.updateSite(formData).subscribe({
        next: (site) => {
          this.loading = false;
          this.router.navigate(['/sites/lista']);
        },
        error: (error) => {
          console.error('Erro ao atualizar site:', error);
          this.loading = false;
        }
      }).add(() => { this.appComponent.hideGlobalLoading(); });
    } else {
      this.siteService.updateSite(formData).subscribe({
        next: (site) => {
          this.loading = false;
          this.router.navigate(['/sites/lista']);
        },
        error: (error) => {
          console.error('Erro ao criar site:', error);
          this.loading = false;
        }
      }).add(() => {
        this.appComponent.hideGlobalLoading();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/sites/lista']);
  }

}
