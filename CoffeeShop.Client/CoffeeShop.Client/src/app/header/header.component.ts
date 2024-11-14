import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { MenuItem } from '../models/auth/menu-item.interface';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {
  isLoggedIn = false;
  isUserMenuOpen = false;
  isModalVisible = false;
  userMenuItems: MenuItem[] = [];
  userName: string = ''; 
  @Input() totalPrice: number = 0;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe();
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      if (this.isLoggedIn) {
        this.loadUserName();
      }
    });
  }

  //checkLoginStatus() {
  //  this.authService.isAuthenticated().subscribe(isAuthenticated => {
  //    this.isLoggedIn = isAuthenticated;
  //    if (isAuthenticated) {
  //      this.loadUserName();
  //     // this.loadMenu();
  //    }
  //  });
  //}

  loadUserName() {
    this.authService.getUserName().subscribe(name => {
      this.userName = name || 'Unknown User';
      console.log(this.userName);
    });
  }



  loadMenu() {
    this.authService.getMenu().subscribe(menuItems => {
      this.userMenuItems = menuItems;
    });
  }

  openModal() {
    this.isModalVisible = true;
  }

  closeModal() {
    this.isModalVisible = false;
  }

  toggleUserMenu() {
    this.isUserMenuOpen = !this.isUserMenuOpen;
  }
}

