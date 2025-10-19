export interface Pagination {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface PagedResponse<T> {
  data: T[];
  pagination: Pagination;
}