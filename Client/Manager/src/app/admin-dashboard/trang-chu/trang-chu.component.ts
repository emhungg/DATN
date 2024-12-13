import { Component } from '@angular/core';
import { AuthService } from 'src/app/service/auth.service';

interface SideNavToggle {
  screenWidth: number;
  collapsed: boolean;
}

@Component({
  selector: 'app-trang-chu',
  templateUrl: './trang-chu.component.html',
  styleUrls: ['./trang-chu.component.css']
})
export class TrangChuComponent {
  isSideNavCollapsed = false;
  screenWidth = 0;

  constructor(private service: AuthService) {
    // user Onl
    // Thiết lập hàm định thời để gọi lại sau mỗi 5 phút
    if (this.service.isUserLoggedIn()) {
      setInterval(() => {
        if (this.service.isUserLoggedIn()) {
          this.service.isUserOnl().subscribe(response => {
            console.log('user-onl');
          }, error => {
            // sessionStorage.clear();
            // window.location.reload();
          });
        }
      }, 5 * 60 * 1000);
    }

  }

  onToggleSideNav(data: SideNavToggle): void {
    this.screenWidth = data.screenWidth;
    this.isSideNavCollapsed = data.collapsed;
  }
}
