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
  isUserMenuOpen = false;
  isModalVisible = false;
  userMenuItems: MenuItem[] = [];
  userName: string = '';
  @Input() totalPrice: number = 0;

  private menuActions: { [key: string]: () => void } = {
    'categories': () => this.router.navigate(['/manage-categories']),
    'orders': () => this.router.navigate(['/orders']),
    'returnToShop': () => this.router.navigate(['']),
    'signOut': () => this.logout()
  };

  constructor(
    public authService: AuthService,
    public cartService: CartService,
    private router: Router
  ) { }

  getCartItemsCount() {
    return this.cartService.cart.selections.length;
  }

  ngOnInit(): void {
    console.log("check auth");
    this.authService.isAuthenticated().subscribe();
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      if (isLoggedIn) {
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
      this.router.navigate(['']);
    });
  }
}

