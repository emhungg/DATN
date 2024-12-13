import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map, of } from 'rxjs';
import { AuthService } from '../service/auth.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private cartItemsSubject: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
  public cartItems$: Observable<any[]> = this.cartItemsSubject.asObservable();

  private cartItems: any[] = [];
  private lastActionTime: number = Date.now();
  constructor(private auth: AuthService, private http: HttpClient) {

    // Set up a timer to check inactivity after 30 minutes
    this.setupInactivityTimer();
  }


  public notifyCartChanged() {
    this.cartItemsSubject.next(this.cartItems);
  }

  getCartItems(): Observable<any[]> {
    if (this.auth.isUserLoggedIn()) {
      const options = {
        headers: new HttpHeaders().append("Authorization", "Bearer " + this.auth.getToken()),
      };

      return this.http.get(environment.apiUrl + '/api/get-cart', options).pipe(
        map((response: any) => {
          this.cartItems = response.response.data.cartData;
          this.notifyCartChanged();
          return this.cartItems;
        }),
        catchError(error => {
          console.error('Error fetching cart items:', error);
          return of([]);
        })
      );
    } else {
      const storedCart = localStorage.getItem('cart');
      if (storedCart) {
        this.cartItems = JSON.parse(storedCart);
        this.notifyCartChanged();
      }
      return of(this.cartItems);
    }
  }

  addToCart(item: any) {
    // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
    const existingItemIndex = this.cartItems.findIndex((cartItem) => cartItem.productId === item.productId);

    if (existingItemIndex !== -1) {
      if (this.cartItems[existingItemIndex].quantity < item.totalQuantity) {
        this.cartItems[existingItemIndex].quantity++;
        this.updateCartItems(this.cartItems);
        this.notifyCartChanged();
      } else {
        console.log('Số lượng sản phẩm đã vượt quá giới hạn.');
      }
    } else {
      if (0 < item.totalQuantity) {
        this.cartItems.push({ ...item });
        this.updateCartItems(this.cartItems);
        this.notifyCartChanged();
      } else {
        console.log('Số lượng sản phẩm đã vượt quá giới hạn.');
      }
    }

    localStorage.setItem('cart', JSON.stringify(this.cartItems));
    this.resetInactivityTimer();
  }

  updateCartItems(items: any[]) {
    // Cập nhật thông tin giỏ hàng
    this.cartItems = [...items];
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
    this.notifyCartChanged()
  }
  clearCart() {
    this.cartItems = [];
    localStorage.removeItem('cart');
    this.notifyCartChanged()

  }

  getDataToSync() {
    // Lấy dữ liệu từ local storage
    const cartDataString = localStorage.getItem('cart');

    // Kiểm tra xem dữ liệu có tồn tại không
    if (cartDataString) {
      // Chuyển đổi dữ liệu từ JSON string sang đối tượng JavaScript
      const cartData = JSON.parse(cartDataString);

      // Ánh xạ dữ liệu theo định dạng payload yêu cầu
      const cartInsertData = {
        cartInsertData: cartData.map((item: any) => {
          return {
            productId: item.productId || '',
            quantity: item.quantity || 0,
            price: item.productSell > 0 ? item.productSell : item.productPrice
          };

        })
      }
      this.clearCart();
      return cartInsertData;
    } else {
      return null;
    }
  }

  syncCartWithDatabase(items: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.auth.getToken()),
    }
    this.notifyCartChanged()
    return this.http.post(environment.apiUrl + '/api/update-cart', items, options)
  }

  setupInactivityTimer() {
    const inactivityPeriod = 30 * 60 * 1000; // 30 minutes in milliseconds
    setTimeout(() => {
      const currentTime = Date.now();
      if (currentTime - this.lastActionTime >= inactivityPeriod) {
        // Xoá giỏ hàng sau 30 phút không hoạt động
        this.clearCart();
      }
    }, inactivityPeriod);
  }

  resetInactivityTimer() {
    this.lastActionTime = Date.now();
  }


  updateQuantity(item: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.auth.getToken()),
    }
    return this.http.put(environment.apiUrl + '/api/calculate-total', item, options)
  }

  deleteItems(items: any): Observable<any> {
    const options = {
      headers: new HttpHeaders().append("Authorization", "Bearer " + this.auth.getToken()),
    }
    return this.http.post(environment.apiUrl + '/api/cart-delete', items, options)
  }

  getTotalQuantity(): number {
    // Tính tổng số lượng từ danh sách giỏ hàng
    return this.cartItems.reduce((total, item) => total + item.quantity, 0);
  }

}
