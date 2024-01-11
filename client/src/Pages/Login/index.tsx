import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

//+ axios(api)
import api from '../../Services/api';

//+ CSS
import './styles.css';

//+ Imagens
//pra funciona, adicionada a 'global', pro typescript entender as imagens como modulos
import logoImage from '../../Assets/logo.svg'
import padlock from '../../Assets/padlock.png'
import { AxiosError } from 'axios';

//interface Props {}

export default function Login() {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const navigate = useNavigate();

    async function login(e: React.FormEvent) {
        e.preventDefault();

        const data = {
            userName, password
        };

        try {
            const response = await api.post('Auth/signin', data);

            localStorage.setItem('userName', userName);
            localStorage.setItem('accesToken', response.data.accesToken);
            localStorage.setItem('refreshToken', response.data.refreshToken);

            navigate('/books')
        } catch (error: any) {
            alert(error.response.data);
        }
    }
    
    return (
        <div className="login-container">
            <section className="form">
                <img src={logoImage} alt="Login Logo" />
                <form onSubmit={login}>
                    <h1>Access your account</h1>
                    
                    <input 
                        type="text" 
                        placeholder='Login'
                        value={userName}
                        onChange={e => setUserName(e.target.value)}
                    />
                    
                    <input 
                        type="password" 
                        placeholder='Senha'
                        value={password}
                        onChange={e => setPassword(e.target.value)}                        
                    />
                    
                    <button type="submit" className='button'>Login</button>                    
                </form>
                
            </section>
            <img src={padlock} alt="Login" />
        </div>
    );
}
