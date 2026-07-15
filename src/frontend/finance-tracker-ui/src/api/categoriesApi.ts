import { type Category } from "../types";

const BASE_URL = 'http://localhost:5011/Categories'

export const categoriesApi = {
    getAll: async (): Promise<Category[]> => {
        const response = await fetch(BASE_URL);
        if (!response.ok) throw new Error('Ошибка при загрузке категорий');
        return response.json();
    },

    getById: async (id: string): Promise<Category> => {
        const response = await fetch(`${BASE_URL}/${id}`)
        if (!response.ok) throw new Error('Ошибка при загрузке категории');
        return response.json();
    },
    
    create: async (category: Omit<Category, 'id'>): Promise<Category> => {
        const response = await fetch(BASE_URL, {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(category)
        })
        if (!response.ok) throw new Error('Ошибка при создании категории');
        return response.json();
    },

    delete: async (id: string): Promise<void> => {
        const response = await fetch(`${BASE_URL}/${id}`,{
            method: 'DELETE'
        })
        if (!response.ok) throw new Error('Ошибка при удалении категории');
    },

    update: async (id: string, name: string): Promise<Category> => {
        const response = await fetch(`${BASE_URL}/${id}`, {
            method: 'PUT',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({name})
        })
        if (!response.ok) throw new Error('Ошибка при изменении категории');
        return response.json();
    }
}