//import React, { ReactNode } from 'react';
import { Link } from 'react-router-dom';
import {FiPower, FiEdit, FiTrash2} from 'react-icons/fi'

//+ CSS
import './styles.css';

//+ Imagens
//pra funciona, adicionada a 'global', pro typescript entender as imagens como modulos
import logoImage from '../../Assets/logo.svg'

//interface Props {}

export default function Books() {
    return (
        <div className="book-container">
            <header>
                <img src={logoImage} alt="Logo" />
                <span>Welcome, <strong>Alexandre</strong>!</span>
                <Link className='button' to="../book/new">Add New Book</Link>
                <button type='button'>
                    <FiPower size={18} color="blueviolet"/>
                </button>
            </header>

            <h1>Registered Books</h1>
            <ul>
                <li>
                    <strong>Title:</strong>
                    <p>Docker</p>
                    <strong>Author:</strong>
                    <p>Oi</p>
                    <strong>Price:</strong>
                    <p>R$ 47,90</p>
                    <strong>Release Date:</strong>
                    <p>12/07/2023</p>
                    <button type='button'>
                        <FiEdit size={20} color="blueviolet"/>
                    </button>
                    <button type='button'>
                        <FiTrash2 size={20} color="blueviolet"/>
                    </button>                    
                </li>

                <li>
                    <strong>Title:</strong>
                    <p>Docker</p>
                    <strong>Author:</strong>
                    <p>Oi</p>
                    <strong>Price:</strong>
                    <p>R$ 47,90</p>
                    <strong>Release Date:</strong>
                    <p>12/07/2023</p>
                    <button type='button'>
                        <FiEdit size={20} color="blueviolet"/>
                    </button>
                    <button type='button'>
                        <FiTrash2 size={20} color="blueviolet"/>
                    </button>                    
                </li>

                <li>
                    <strong>Title:</strong>
                    <p>Docker</p>
                    <strong>Author:</strong>
                    <p>Oi</p>
                    <strong>Price:</strong>
                    <p>R$ 47,90</p>
                    <strong>Release Date:</strong>
                    <p>12/07/2023</p>
                    <button type='button'>
                        <FiEdit size={20} color="blueviolet"/>
                    </button>
                    <button type='button'>
                        <FiTrash2 size={20} color="blueviolet"/>
                    </button>                    
                </li>                                
            </ul>

        </div>
    );
}
