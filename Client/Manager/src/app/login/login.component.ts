import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginFormGroup!: FormGroup;
  loadingTitle = "Loading...";
  constructor(private router: Router, private formBuilder: FormBuilder, private service: AuthService, private toastr: ToastrService, private spinner: NgxSpinnerService) {
    if (service.isUserLoggedIn()) {
      this.router.navigate(['/admin/dashboard']);
    } else {
      sessionStorage.clear();
    }
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.loginFormGroup = this.formBuilder.group({
      userID: this.formBuilder.control('', Validators.required),
      password: this.formBuilder.control('', Validators.required),
    });
  }


  response: any;
  error: any;
  async isProcessedLogin() {
    if (this.loginFormGroup.valid) {
      this.loadingTitle = "Signing in...";
      await this.spinner.show();
      try {
        await this.service.doLogin(this.loginFormGroup.value).subscribe(res => {
          this.response = res;
          const tokens = this.response.response.data.token.split(".");
          const tokenFinal = tokens.slice(0, tokens.length - 1).join(".");
          if (this.response.response.data.role === 1) {
            sessionStorage.setItem('token_admin', tokenFinal);
            this.router.navigate(['/admin/dashboard']);
          } else {
            this.toastr.error("Sai tài khoản hoặc mật khẩu", 'Error');
          }
          this.spinner.hide();
        }, (error) => {
          this.error = error;
          if (error.status === 400) {
            this.toastr.error(this.error.error.response.data.errorMessage, 'Error');
          } else {
            this.toastr.warning("Connection to server error", 'Error');
          }
          this.spinner.hide();
        });

      } catch (error) {
        console.log(error);
        this.toastr.error("Login fail: " + JSON.stringify(error), 'Error')
      }

    } else {
      this.toastr.error('Nhập đầy đủ thông tin')
    }
  }
}
