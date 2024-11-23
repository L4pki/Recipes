import React from 'react';
import { Tag } from '../../types/recipe';

interface PopularTagsProps {
    tags: Tag[]; // Должен быть массив тегов
    onTagClick: (tag: string) => void;
}

const PopularTags: React.FC<PopularTagsProps> = ({ tags, onTagClick }) => {
    // Проверяем, что tags является массивом
    if (!Array.isArray(tags)) {
        console.error('tags is not an array:', tags);
        return <div>Ошибка: Теги не загружены.</div>; // Обработка ошибки
    }

    return (
        <div>
            <h3>Популярные теги</h3>
            <ul>
                {tags.map(tag => (
                    <li key={tag.id} onClick={() => onTagClick(tag.name)}>
                        {tag.name}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default PopularTags;
