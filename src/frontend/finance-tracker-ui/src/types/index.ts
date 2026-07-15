export type TransactionType = 'Income' | 'Expense';

export interface Category {
    id: string;
    name: string;
    type: string; // 'Income' | 'Expense'
}

export interface Transaction {
    id: string;
    amount: number;
    description?: string;
    date: string;
    categoryId: string;
    categoryName: Category;
}