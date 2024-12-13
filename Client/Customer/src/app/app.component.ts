import { Component, HostListener, OnInit, isDevMode } from '@angular/core';
import { AuthService } from './service/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Tshops';

  constructor(public auth: AuthService) {
    // sessionStorage.clear();
  }

  ngOnInit() {

    // user login-onl check
    this.isUserOnl()
  }

  // user Onl
  isUserOnl() {
    // Thiết lập hàm định thời để gọi lại sau mỗi 5 phút
    if (this.auth.isUserLoggedIn()) {
      setInterval(() => {
        if (this.auth.isUserLoggedIn()) {
          this.auth.isUserOnl().subscribe(response => {
            console.log('user-onl');
          }, error => {
            sessionStorage.removeItem('token');
            window.location.reload();
          });
        }
      }, 5 * 60 * 1000);
    }
  }


  isShow!: boolean;
  topPosToStartShowing = 100;

  @HostListener('window:scroll')
  checkScroll() {

    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;

    if (scrollPosition >= this.topPosToStartShowing) {
      this.isShow = true;
    } else {
      this.isShow = false;
    }
  }

  // TODO: Cross browsing
  gotoTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth'
    });
  }

}
