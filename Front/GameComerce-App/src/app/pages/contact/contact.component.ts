// contact.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SiteInfo } from 'src/app/model/siteInfo';
import { SiteInfoService } from 'src/app/services/SiteInfo.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnInit {

  siteInfo?: SiteInfo;
  currentDate: Date = new Date();
  contactForm: FormGroup;
  submitted: boolean = false;
  isLoading: boolean = false;

  constructor(
    private siteInfoService: SiteInfoService,
    private fb: FormBuilder
  ) {
    this.contactForm = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      mensagem: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  ngOnInit(): void {
    this.siteInfoService.getSiteInfo().subscribe(data => {
      this.siteInfo = data;
    });
  }

  onSubmit(): void {
    if (this.contactForm.valid) {
      this.isLoading = true;
      
      // Simula envio do formulário (substituir por API real depois)
      setTimeout(() => {
        this.isLoading = false;
        this.submitted = true;
        this.contactForm.reset();
      }, 1500);
    } else {
      // Marca todos os campos como touched para mostrar erros
      Object.keys(this.contactForm.controls).forEach(key => {
        this.contactForm.get(key)?.markAsTouched();
      });
    }
  }

  get f() { return this.contactForm.controls; }

  voltarParaFormulario(): void {
    this.submitted = false;
  }
}