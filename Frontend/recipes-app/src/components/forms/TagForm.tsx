import React from 'react';
import { Tag } from '../../types/recipe';
import './TagForm.css';

interface PopularTagsProps {
    tags: Tag[];
    onTagClick: (tag: string) => void;
}

const PopularTags: React.FC<PopularTagsProps> = ({ tags, onTagClick }) => {
    if (!Array.isArray(tags)) {
        console.error('tags is not an array:', tags);
        return <div>Ошибка: Теги не загружены.</div>;
    }

    return (
        <div>
            <ul className='popular-tag-list'>
                {tags.map(tag => (
                    <li className='popular-tag' key={tag.id} onClick={() => onTagClick(tag.name)}>
                        {tag.name}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default PopularTags;
