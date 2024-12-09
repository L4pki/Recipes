import React from 'react';
import { Link } from 'react-router-dom';

const Navbar: React.FC = () => {
    return (
        <nav>
            <ul>
                <li><Link to="/authorization">Авторизация</Link></li>
                <li><Link to="/profile">Профиль</Link></li>
                <li><Link to="/recipes">Рецепты</Link></li>
                <li><Link to="/favorite">Избранное</Link></li>
                <li><Link to="/main">Главная</Link></li>
                <li><Link to="/addRecipe">Добавление</Link></li>
                <li><Link to="/detail/">Детали</Link></li>
            </ul>
        </nav>
    );
};

export default Navbar;