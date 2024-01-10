import React from 'react'
import { Link } from 'react-router-dom';
import {FiArrowDownLeft} from 'react-icons/fi'
//+ CSS
import './styles.css';

//+ Imagens
//pra funciona, adicionada a 'global', pro typescript entender as imagens como modulos
import logoImage from '../../Assets/logo.svg'

//interface Props {}

export default function NewBook () {
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

            <form>
              <input placeholder="Title" />
              <input placeholder="Author" />
              <input type="Date" />
              <input placeholder="Price" />

              <button type="submit" className='button'>Add</button>
            </form>
        </div>
    </div>
  )
}