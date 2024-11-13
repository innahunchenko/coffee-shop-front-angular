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
    this.checkLoginStatus();
    console.log('Initial login status:', this.isLoggedIn);
  }

  checkLoginStatus() {
    this.authService.isAuthenticated().subscribe(isAuthenticated => {
      this.isLoggedIn = isAuthenticated;
      if (isAuthenticated) {
        this.loadUserName();
        this.loadMenu();
      }
    });
  }

  loadUserName() {
    this.authService.getUserName().subscribe(name => {
      this.userName = name; 
    });
  }


  loadMenu() {
    //this.authService.getMenu().subscribe(menuItems => {
    //  this.userMenuItems = menuItems;
    //});
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

