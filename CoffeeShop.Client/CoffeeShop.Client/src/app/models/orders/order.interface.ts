export interface OrderItem {
  productId: string;
  productName: string;
  quantity: number;
  price: number;
}

export interface Order {
  orderName: string;
  orderStatus: string;
  orderItems: OrderItem[];
  totalPrice: number;
  createdAt: string;
}

