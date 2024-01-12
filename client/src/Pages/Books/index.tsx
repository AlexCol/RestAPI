import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import {FiPower, FiEdit, FiTrash2} from 'react-icons/fi'

//+ CSS
import './styles.css';

//+ Imagens
//pra funciona, adicionada a 'global', pro typescript entender as imagens como modulos
import logoImage from '../../Assets/logo.svg'
import { AxiosRequestConfig } from 'axios';
import api from '../../Services/api';


//interface Props {}

export default function Books() {
    interface book {
        id: number,
        title: string,
        author: string,
        launchDate: string,
        price: number
    }
    const [books, setBooks] = useState<book[]>([]);
    const [page, setPage] = useState(1);

    const userName = localStorage.getItem("userName");
    const navigate = useNavigate();
    
    const accesToken = localStorage.getItem('accesToken');
    const configAxios: AxiosRequestConfig = {
        headers: {
          Authorization: `Bearer ${accesToken}`
        }
    }

    useEffect(() => {
        fechtMoreBooks();
    }, [accesToken]);

    async function fechtMoreBooks() {
        const response:any = await api.get(`/Book/asc/4/${page}`, configAxios);
        setBooks([...books, ...response.data.list]);
        setPage(page+1);
    }
    
    async function logout() {
        try {
            await api.get(`Auth/revoke`, configAxios);
            localStorage.clear();
            navigate('/');
        } catch (error) {
            alert(error);
        }        
    }
    
    async function deleteBook(id: number) {
        try {
            await api.delete(`book/${id}`, configAxios);
            setBooks(books.filter(book => book.id !== id))
        } catch (error) {
            alert(error);
        }
    }

    async function editBook(id: number) {
        try {
            navigate(`../book/new/${id}`);
        } catch (error) {
            alert("Edit failed.");
        }
    }

    return (
        <div className="book-container">
            <header>
                <img src={logoImage} alt="Logo" />
                <span>Welcome, <strong>{userName?.toLocaleUpperCase()}</strong>!</span>
                <Link className='button' to="../book/new/0">Add New Book</Link>
                <button onClick={logout} type='button'>
                    <FiPower size={18} color="blueviolet"/>
                </button>
            </header>

            <h1>Registered Books</h1>
            <ul>
                {books.map(book => (
                <li key={book.id}>
                    <strong>Title:</strong>
                    <p>{book.title}</p>
                    <strong>Author:</strong>
                    <p>{book.author}</p>
                    <strong>Price:</strong>
                    <p>{Intl.NumberFormat('pt-Br', {style: 'currency', currency: 'BRL'}).format(book.price)}</p>
                    <strong>Release Date:</strong>
                    <p>{Intl.DateTimeFormat('pt-Br').format(new Date(book.launchDate))}</p>
                    <button onClick={() => editBook(book.id)} type='button'>
                        <FiEdit size={20} color="blueviolet"/>
                    </button>
                    <button onClick={() => deleteBook(book.id)} type='button'>
                        <FiTrash2 size={20} color="blueviolet"/>
                    </button>                    
                </li>                         
                ))}                
            </ul>
            <button className='button' onClick={fechtMoreBooks} type='button'>Load more</button>
        </div>
    );
}
