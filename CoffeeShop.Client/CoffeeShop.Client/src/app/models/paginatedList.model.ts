export class PaginatedList<T> {
  constructor(
    public items: T[] = [],
    public pageIndex: number = 1,
    public totalPages: number = 0,
    public totalItems: number = 0) { }
}
