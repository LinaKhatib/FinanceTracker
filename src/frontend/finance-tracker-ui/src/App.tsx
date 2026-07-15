// src/App.tsx
import Categories from './pages/Categories';
import Transactions from './pages/Transactions';
import { Routes, Route, NavLink, useLocation  } from 'react-router-dom';
import { AnimatePresence, motion } from 'framer-motion';

function App() {
  const location = useLocation();
  return (
    <div className="min-h-screen flex flex-col">
      <header className="bg-light-yellow  sticky top-0 z-10">
        <div className="max-w-5xl mx-auto px-4 h-16 flex items-center justify-between">
          <div className="flex items-center gap-2 font-bold text-xl text-text-brown">
            <span>FinanceTracker</span>
          </div>
          <span className="text-xs bg-cream text-rich-orange px-3 py-1 rounded-full font-medium">
            Режим разработки (Категории)
          </span>
        </div>
      </header>

      <div className="flex flex-row w-full mx-auto max-w-screen-2xl my-4 gap-x-6">
        <nav className="flex-1 self-start h-auto bg-cream rounded-2xl overflow-hidden p-8 flex flex-col gap-6">
          
          <NavLink
            to="/transactions"
            className={({ isActive }) =>
              `flex items-center justify-center bg-light-yellow w-full h-15  rounded-2xl text-3xl text-text-brown ${
                isActive 
                  ? 'bg-rich-orange' 
                  : 'hover:bg-light-yellow hover:border-2 hover:border-rich-orange'
              }`
            }
          >
            Транзакции
          </NavLink>
          
          <NavLink
            to="/categories"
            className={({ isActive }) =>
              `flex items-center justify-center bg-light-yellow w-full h-15  rounded-2xl text-3xl text-text-brown ${
                isActive 
                  ? 'bg-rich-orange' 
                  : 'hover:bg-light-yellow hover:border-2 hover:border-rich-orange'
              }`
            }
          >
            Категории
          </NavLink>
        </nav>

        <main className="flex-4">
          <AnimatePresence mode="wait">
            <Routes location={location} key={location.pathname}>
              <Route path="/transactions" 
                element={
                  <motion.div
                    initial={{ opacity: 0, y: 10 }}
                    animate={{ opacity: 1, y: 0 }}
                    exit={{ opacity: 0, y: -10 }}
                    transition={{ 
                      duration: 0.15,
                      ease: "easeInOut"
                    }}>
                    <Transactions />
                  </motion.div>
                } />
              <Route
                path="/categories"
                element={
                  <motion.div
                    initial={{ opacity: 0, y: 10 }}
                    animate={{ opacity: 1, y: 0 }}
                    exit={{ opacity: 0, y: -10 }}
                    transition={{ 
                      duration: 0.2,
                      ease: "easeInOut"
                    }}>
                    <Categories />
                  </motion.div>
                }/>
            </Routes>
          </AnimatePresence>
        </main>
        </div>
    </div>
  )
}

export default App 