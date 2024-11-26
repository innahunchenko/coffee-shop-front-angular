import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { MenuItem } from '../models/auth/menu-item.interface';
import { CartService } from '../services/cart/cart.service';
import { Router } from '@angular/router';

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

  private menuActions: { [key: string]: () => void } = {
    'manageCatalog': () => this.router.navigate(['/manage-catalog']),
    'manageUsers': () => this.router.navigate(['/manage-users']),
    'manageUserOrders': () => this.router.navigate(['/manage-user-orders']),
    'orderHistory': () => this.router.navigate(['/order-history']),
    'profile': () => this.router.navigate(['/profile']),
    'returnToShop': () => this.router.navigate(['']),
    'signOut': () => this.logout()
  };

  constructor(
    private authService: AuthService,
    public cartService: CartService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe();
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      if (this.isLoggedIn) {
        this.loadUserName();
        this.loadMenu();
      }
    });
  }

  loadUserName() {
    this.authService.getUserName().subscribe(name => {
      this.userName = name || 'Unknown User';
    });
  }

  loadMenu() {
    this.authService.getMenu().subscribe(menuItems => {
      this.userMenuItems = menuItems.map(item => {
        if (this.menuActions[item.id]) {
          item.action = this.menuActions[item.id];
        } else {
          item.action = () => { };
        }
        return item;
      });
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

  logout(): void {
    this.authService.logout().subscribe(() => {
      this.isLoggedIn = false;
      this.router.navigate(['']);
    });
  }
}

