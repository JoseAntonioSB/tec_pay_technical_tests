export interface Product {
    id: string;
    name: string;
    description?: string;
    price: number;
    stock: number;
    isActive: boolean;
    categoryId: string;
    categoryName: string;
    createdAt: string;
    updatedAt?: string;
}

export interface Category {
    id: string;
    name: string;
    description?: string;
}

export interface CreateCategoryRequest {
    name: string;
    description?: string;
}

export interface UpdateCategoryRequest {
    name: string;
    description?: string;
}

export interface CreateProductRequest {
    name: string;
    description?: string;
    price: number;
    stock: number;
    categoryId: string;
}

export interface UpdateProductRequest {
    name: string;
    description?: string;
    price: number;
    stock: number;
    categoryId: string;
}

export interface ProductFilter {
    search?: string;
    categoryId?: string;
    isActive?: boolean;
    page: number;
    pageSize: number;
}
