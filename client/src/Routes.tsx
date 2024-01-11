//import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import Login from './Pages/Login';
import Books from './Pages/Books';
import NewBook from './Pages/NewBook';

export default function AppRoutes() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path='/' element={<Login />}/>
                <Route path='/books' element={<Books />}/>
                <Route path='/book/new/:bookId' element={<NewBook />}/>
            </Routes>
        </BrowserRouter>
    );
}
