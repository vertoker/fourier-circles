using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Arrow
{
    private const double PI2ANGLE = 57.2957795130823209d;

    private Transform _transform;
    private SpriteRenderer _sprite_renderer;

    public Arrow(Transform instance, Transform parent)
    {
        _transform = Object.Instantiate(instance, parent);
        _sprite_renderer = _transform.GetComponent<SpriteRenderer>();
    }

    public void Transformation(double xs, double ys, double xe, double ye)
    {
        float posX = (float)(xe - xs), posY = (float)(ye - ys);
        float angle = (float)(Mathf.Atan2(posY, posX) * PI2ANGLE) - 90f;
        float sca = (float)Mathf.Sqrt(posX * posX + posY * posY);
        float middlePosX = (float)xs + posX / 2f;
        float middlePosY = (float)ys + posY / 2f;

        _transform.localPosition = new Vector3(middlePosX, middlePosY, 0);
        _transform.localEulerAngles = new Vector3(0, 0, angle);
        _sprite_renderer.size = new Vector2(1, sca);
    }

    public void DestroyArrow()
    {
        Object.Destroy(_transform.gameObject);
    }
}