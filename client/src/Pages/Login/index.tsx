//import React, { ReactNode } from 'react';

//+ CSS
import './styles.css';

//+ Imagens
//pra funciona, adicionada a 'global', pro typescript entender as imagens como modulos
import logoImage from '../../Assets/logo.svg'
import padlock from '../../Assets/padlock.png'

//interface Props {}

export default function Login() {
    return (
        <div className="login-container">
            <section className="form">
                <img src={logoImage} alt="Login Logo" />
                <form>
                    <h1>Access your account</h1>
                    <input type="text" placeholder='Login'/>
                    <input type="password" placeholder='Senha'/>
                    <button type="submit" className='button'>Login</button>                    
                </form>
                
            </section>
            <img src={padlock} alt="Login" />
        </div>
    );
}
