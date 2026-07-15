import { useEffect, useState } from 'react';
import { Plus, X, Astroid, ChevronDown } from 'lucide-react';
import { type Category } from '../types';
import { categoriesApi } from '../api/categoriesApi';

import { Listbox } from '@headlessui/react';


const options = ['Зачисление', 'Списание'];


export default function Categories() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [name, setName] = useState(''); 
  const [selected, setSelected] = useState(options[0]);
  const [isLoading, setIsLoading] = useState(false);


  useEffect(() => {
    loadCategories();
  }, []); 

  const loadCategories = async () => {
    try {
      setLoading(true);
      setError(null); 
      const data = await categoriesApi.getAll();
      setCategories(data);
    } catch (err: any) {
      setError('Ошибка загрузки категорий: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (confirm('Удалить эту категорию?')) {
      try {
        setError(null);
        await categoriesApi.delete(id);
        setCategories(categories.filter(c => c.id !== id));
      } catch (err: any) {
        setError('Ошибка удаления категории: ' + err.message);
      }
    }
  };

  const handleCreate = async () => {
    if (!name.trim()) {
      alert('Введите название категории');
      return;
    }

    const newCategory = { name: name.trim(), type: selected === 'Зачисление' ? 'Income' : 'Expense' };

    try {
      setIsLoading(true);
      setError(null);
      const createdCategory = await categoriesApi.create(newCategory);
      setCategories([...categories, createdCategory]);
      setName(''); 
    } catch (err: any) {
      setError('Ошибка создания категории: ' + err.message);
    } finally{
      setIsLoading(false);
    }
  }

  
  if (loading) return 
    <div className="text-center py-10 flex flex-col items-center justify-center text-4xl text-light-yellow">
      Загрузка категорий...
    </div>;
  if (error) return <div className="text-red-500 text-center py-10">Ошибка: {error}</div>;

  return (
    
    <div className="flex w-full bg-cream rounded-2xl overflow-hidden p-8 gap-6 " >
      
        {/*Центральная колонка с списком категорий */}
      <div className="flex-2 bg-light-yellow rounded-2xl h-fit">
        <div className="h-16 rounded-lg flex items-center px-4 gap-4">
          <button className="btn bg-rich-orange h-7 px-4 rounded-xl text-indigo-600 text-sm">
              Все
          </button>
          <button className="btn bg-rich-orange h-7 px-4 rounded-xl text-cream text-sm">
              Списания
          </button>
          <button className="btn bg-rich-orange h-7 px-4 rounded-xl text-cream text-sm">
              Зачисления
          </button>
        </div>

        <div className="flex-1 flex flex-col rounded-lg mt-2 p-2 divide-y divide-cream">
            {categories.length === 0 ? (
              <p className="p-6 text-center text-slate-400">Категорий пока нет. Создайте первую!</p>
            ) : (
              categories.map(category => (
                <div key={category.id} className="p-4 flex items-center justify-between">
                  <div className="flex items-center gap-5">
                    <div className="p-2 bg-light-yellow text-indigo-600 rounded-lg">
                      <Astroid size={20} />
                    </div>
                    
                    <span className="text-lg font-medium text-text-brown">{category.name}</span>
                  </div>

                  <div className="flex items-center gap-8">
                    <span className={`px-2 py-1 rounded-xl font-medium bg-cream ${
                      category.type === 'Income' ? 'text-emerald-700' : 'text-rose-700'
                    }`}>
                      {category.type === 'Income' ? 'Доход' : 'Расход'}
                    </span>

                    <button onClick={() => handleDelete(category.id)}
                      className="text-rich-orange hover:text-rose-600 p-1 transition-colors">
                      <X size={25} />
                    </button>
                  </div> 
                </div> 
              ))
            )}
        </div>
      </div> 

      {/*Правая колонка с функционалом созданиия новых категорий */}
      <div className="flex-1 flex flex-col items-center justify-center bg-light-yellow rounded-2xl h-[750px] px-4 ">
        <div className="flex-1 flex flex-col items-center justify-center gap-20">

          <div className="w-70 space-y-20">
            {/*Строка ввода для названия категории*/}
            <input type='text' placeholder="Название категории" 
              value={name}
              onChange={(e) => setName(e.target.value)}
              onKeyDown={(e) => e.key === 'Enter' && handleCreate()}
              disabled={isLoading}
              className="border-b-3 border-rich-orange bg-transparent px-2 py-1  focus:outline-none w-full text-text-brown text-xl"/>
              
            {/*Выпадающий список с типом тринзакции категории*/}
            <Listbox value={selected} onChange={setSelected}>
              {({ open }) => (
                <div className="relative">

                  <Listbox.Button className="w-full border-b-3 border-rich-orange px-2 py-1 text-text-brown text-xl text-left bg-transparent flex items-center justify-between
                  !outline-none focus:!outline-none focus-visible:!outline-none"
                      style={{ outline: 'none', boxShadow: 'none' }}>
                        
                    <span>{selected}</span>

                    <ChevronDown className={`h-5 w-5 text-text-brown transition-transform duration-200 ${
                        open ? 'rotate-180' : ''}`} 
                      aria-hidden="true"/>
                  </Listbox.Button>

                  <Listbox.Options className="absolute left-0 right-0 mt-1 text-text-brown bg-cream rounded-xl shadow-lg overflow-hidden z-10 w-full">
                    {options.map((option) => (
                      <Listbox.Option
                        key={option}
                        value={option}
                        className="px-4 py-2 hover:text-cream hover:bg-rich-orange cursor-pointer transition-colors">
                        {option}
                      </Listbox.Option>
                    ))}
                  </Listbox.Options>

                </div> 
              )}
            </Listbox>
          </div>
        </div>

          {/*Кнопка добавления новой категории*/}
        <button 
          onClick={handleCreate}
          disabled={isLoading}
          className="btn flex items-center justify-center bg-rich-orange h-15 w-25 px-4 mb-10 rounded-2xl text-cream text-sm mt-auto">
            <Plus size={30}/>
        </button>
      </div>
    </div>
  );
}