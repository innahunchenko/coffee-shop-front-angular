import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { Order } from "../../models/orders/order.interface";

const API_BASE_URL = 'https://localhost:7075/ordering';
const ordersUrl = `${API_BASE_URL}/orders`;

@Injectable()
export class OrderRepository {
  constructor(private http: HttpClient) { }

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(ordersUrl).pipe(
      map(dtos =>
        dtos.map(dto => ({
          orderName: dto.orderName,
          orderStatus: dto.orderStatus,
          orderItems: dto.orderItems.map(item => ({
            productId: item.productId,
            productName: item.productName,
            quantity: item.quantity,
            price: item.price
          })),
          totalPrice: dto.totalPrice,
          createdAt: dto.createdAt
        }))
      )
    );
  }
}
