import React, { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';

export default function EditBirthday() {
  const { id } = useParams();
  const [fullName, setFullName] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState('');
  const [photo, setPhoto] = useState(null);
  const [photoPreview, setPhotoPreview] = useState(null);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetch(`/api/birthdaypeople/${id}`)
      .then(res => {
        if (!res.ok) throw new Error('Ошибка загрузки данных');
        return res.json();
      })
      .then(data => {
        setFullName(data.fullName);
        setDateOfBirth(data.dateOfBirth.split('T')[0]);
        if (data.photoBase64 && data.photoMimeType) {
          setPhotoPreview(`data:${data.photoMimeType};base64,${data.photoBase64}`);
        }
      })
      .catch(err => setError(err.message));
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    if (!fullName || !dateOfBirth) {
      setError('Заполните все обязательные поля');
      return;
    }

    const formData = new FormData();
    formData.append('FullName', fullName);
    formData.append('DateOfBirth', dateOfBirth);
    if (photo) {
      formData.append('Photo', photo);
    }

    try {
      const res = await fetch(`/api/birthdaypeople/${id}`, {
        method: 'PUT',
        body: formData,
      });

      if (!res.ok) {
        const data = await res.json();
        let msg = 'Ошибка при обновлении';
        if (data && data.errors) {
          msg = Object.values(data.errors).flat().join('\n');
        } else if (data && data.title) {
          msg = data.title;
        }
        throw new Error(msg);
      }

      navigate('/');
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="p-4 max-w-md mx-auto">
      <div className="mb-4">
        <Link to="/" className="text-blue-600 hover:underline">&larr; На главную</Link>
      </div>
      <h1 className="text-2xl font-bold mb-4">Редактировать день рождения</h1>
      {error && <div className="mb-4 text-red-600 whitespace-pre-line">{error}</div>}
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <label className="flex flex-col">
          Полное имя*
          <input
            type="text"
            value={fullName}
            onChange={e => setFullName(e.target.value)}
            className="border rounded p-2"
            required
            minLength={2}
            maxLength={100}
          />
        </label>
        <label className="flex flex-col">
          Дата рождения*
          <input
            type="date"
            value={dateOfBirth}
            onChange={e => setDateOfBirth(e.target.value)}
            className="border rounded p-2"
            required
            max={new Date().toISOString().split('T')[0]}
          />
        </label>
        <label className="flex flex-col">
          Фото (JPEG или PNG, до 2 МБ)
          <input
            type="file"
            accept="image/jpeg,image/png"
            onChange={e => {
              setPhoto(e.target.files[0]);
              if (e.target.files[0]) {
                const reader = new FileReader();
                reader.onload = () => setPhotoPreview(reader.result);
                reader.readAsDataURL(e.target.files[0]);
              } else {
                setPhotoPreview(null);
              }
            }}
          />
        </label>
        {photoPreview && (
          <img
            src={photoPreview}
            alt="Фото"
            className="w-24 h-24 rounded-full object-cover mb-2"
          />
        )}
        <button
          type="submit"
          className="bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
        >
          Сохранить
        </button>
      </form>
    </div>
  );
}