export class PaginatedList<T> {
  constructor(
    public items: T[] = [],
    public totalPages: number = 0) { }
}
