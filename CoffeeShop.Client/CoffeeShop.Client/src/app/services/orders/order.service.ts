import { Injectable } from "@angular/core";
import { OrderRepository } from "./order.repository";
import { Order } from "../../models/orders/order.interface";
import { Observable } from "rxjs";

@Injectable()
export class OrderService {
  constructor(private repository: OrderRepository) { }
  getOrders(): Observable<Order[]> {
    return this.repository.getOrders();
  }
}
