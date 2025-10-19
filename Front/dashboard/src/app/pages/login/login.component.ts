import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  credentials = {
    email: '',
    password: '',
    remember: false
  };

  isLoading = false;
  isPasswordVisible = false; // ← Nova variável

  constructor(private router: Router) {}

  onLogin() {
    if (this.isLoading) return;

    // Validação básica
    if (!this.credentials.email || !this.credentials.password) {
      alert('Por favor, preencha todos os campos.');
      return;
    }

    this.isLoading = true;

    // Simulação de login (substituir por chamada HTTP real)
    setTimeout(() => {
      this.isLoading = false;
      
      // Login mock - substituir por validação real
      if (this.credentials.email === 'rbarbosa@gmail.com' && this.credentials.password === 'Carros10@') {
        // Salvar no localStorage se "lembrar de mim" estiver marcado
        if (this.credentials.remember) {
          localStorage.setItem('user', JSON.stringify({
            email: this.credentials.email,
            remember: true
          }));
        } else {
          sessionStorage.setItem('user', JSON.stringify({
            email: this.credentials.email
          }));
        }
        
        // Redirecionar para o dashboard
        this.router.navigate(['/dashboard']);
      } else {
        alert('E-mail ou senha incorretos. Tente: admin@dashvista.com / admin123');
      }
    }, 1500);
  }

  // Novo método para mostrar/esconder senha
  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
  }

  onRememberChange(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target) {
      this.credentials.remember = target.checked;
    }
  }

  ngOnInit() {
    // Verificar se há credenciais salvas
    const savedUser = localStorage.getItem('user') || sessionStorage.getItem('user');
    if (savedUser) {
      const user = JSON.parse(savedUser);
      this.credentials.email = user.email;
      this.credentials.remember = user.remember || false;
    }
  }
}