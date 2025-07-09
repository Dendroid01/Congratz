import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';

export default function BirthdayList({ endpoint }) {
  const [people, setPeople] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const fetchData = () => {
    setLoading(true);
    fetch(endpoint)
      .then(res => {
        if (!res.ok) throw new Error('Ошибка загрузки данных');
        return res.json();
      })
      .then(data => {
        setPeople(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchData();
  }, [endpoint]);

  const handleDelete = async (id) => {
    if (!window.confirm('Удалить запись?')) return;
    try {
      const res = await fetch(`/api/birthdaypeople/${id}`, { method: 'DELETE' });
      if (!res.ok) throw new Error('Ошибка при удалении');
      fetchData(); // обновить список после удаления
    } catch (err) {
      alert(err.message);
    }
  };

  if (loading) return <div>Загрузка...</div>;
  if (error) return <div className="text-red-600">Ошибка: {error}</div>;
  if (people.length === 0) return <div>Записей нет</div>;

  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
      {people.map(person => (
        <div key={person.id} className="border p-4 rounded shadow flex items-center gap-4">
          {person.photoBase64 ? (
            <img
              src={`data:${person.photoMimeType};base64,${person.photoBase64}`}
              alt={person.fullName}
              className="w-16 h-16 rounded-full object-cover"
            />
          ) : (
            <div className="w-16 h-16 rounded-full bg-gray-300 flex items-center justify-center text-gray-600">
              Фото
            </div>
          )}
          <div className="flex-1">
            <div className="font-semibold">{person.fullName}</div>
            <div className="text-sm text-gray-500">
              {new Date(person.dateOfBirth).toLocaleDateString()}
            </div>
          </div>
          <div className="flex flex-col gap-1">
            <button
              className="text-blue-600 hover:underline text-sm"
              onClick={() => navigate(`/edit/${person.id}`)}
            >
              Редактировать
            </button>
            <button
              className="text-red-600 hover:underline text-sm"
              onClick={() => handleDelete(person.id)}
            >
              Удалить
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}