import React, { useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom';
import {FiArrowDownLeft} from 'react-icons/fi'


//+ axios(api)
import api from '../../Services/api';
//+ CSS
import './styles.css';

//+ Imagens
//pra funciona, adicionada a 'global', pro typescript entender as imagens como modulos
import logoImage from '../../Assets/logo.svg'
import { AxiosRequestConfig } from 'axios';

//interface Props {}

export default function NewBook () {
  const [id, setId] = useState(0);
  const [title, setTitle] = useState('');
  const [author, setAuthor] = useState('');
  const [launchDate, setLaunchDate] = useState('');
  const [price, setPrice] = useState('');
  
  const { bookId } = useParams();

  //! preparo o cabeçalho com o token que será usado nas requisicoes
  const accesToken = localStorage.getItem('accesToken');
  const configHeader: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${accesToken}`
    }
  }

  const navigate = useNavigate();

  useEffect(() => {
    if(bookId === "0") return;
    
    loadBook();
  }, [bookId]);

  async function loadBook() {
    try {
      const response = await api.get(`book/${bookId}`, configHeader);
      let ajustedDate = response.data.launchDate.split("T", 10)[0];

      setId(response.data.id);
      setTitle(response.data.title);
      setAuthor(response.data.author);
      
      setLaunchDate(ajustedDate);
      setPrice(response.data.price);

    } catch (error) {
      alert("Erro ao carregar livro.");
      navigate('/books');
    }
  }
  
  async function CreateNewBook(e: React.FormEvent) {
    e.preventDefault();

    const data = {
      title,
      author,
      launchDate,
      price
    };

    try {      
      await api.post('Book', data, configHeader);

      alert("Livro salvo com Sucesso!");
      navigate("/books")
    } catch (error) {
      alert(error);
    }
  }
  
  return (
    <div className="new-book-container">
        <div className="content">
            <section className="form">
                <img src={logoImage} alt="Logo"/>
                <h1>Add New Book</h1>
                <p>Enter the book information and click on 'Add'!</p>
                <Link className='back-link' to="/books">
                    <FiArrowDownLeft size={16} color="blueviolet"/>
                </Link>
            </section>

            <form onSubmit={CreateNewBook}>
              <input
                placeholder="Title" 
                value={title}
                onChange={e => setTitle(e.target.value)}
                required
              />
              <input
                placeholder="Author" 
                value={author}
                onChange={e => setAuthor(e.target.value)}
                required
              />
              <input
                type="Date" 
                value={launchDate}
                onChange={e => setLaunchDate(e.target.value)}
                required
              />
              <input
                placeholder={"Price"}
                value={
                  (typeof(price) === "number") ? price :
                  (isNaN(Number(price.replace(",", "."))) ? '' : price)
                }
                onChange={e => setPrice(e.target.value.replace(",", "."))}
                required
              />

              <button type="submit" className='button'>Add</button>
            </form>
        </div>
    </div>
  )
}