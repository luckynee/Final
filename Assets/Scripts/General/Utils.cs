using UnityEngine;

namespace PokerSeed.General
{
    public static class Utils
    {
        public static bool Overlaps(this RectTransform _rectTransform1, RectTransform _rectTransform2)
        {
            Vector3[] corners = new Vector3[4];
            _rectTransform1.GetWorldCorners(corners);
            Rect rec = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

            _rectTransform2.GetWorldCorners(corners);
            Rect rec2 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

            if (rec.Overlaps(rec2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool FullyContains(RectTransform _objectTransform, RectTransform _boundaryTransform)
        {
            var rect = GetObjectWorldRect(_boundaryTransform);
            var otherRect = GetObjectWorldRect(_objectTransform);

            // Now that we have the world space rects simply check
            // if the other rect lies completely between min and max of this rect
            return rect.xMin <= otherRect.xMin
                && rect.yMin <= otherRect.yMin
                && rect.xMax >= otherRect.xMax
                && rect.yMax >= otherRect.yMax;
        }
        private static Rect GetObjectWorldRect(RectTransform _rectTransform)
        {
            // This returns the world space positions of the corners in the order
            // [0] bottom left,
            // [1] top left
            // [2] top right
            // [3] bottom right
            var corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);

            Vector2 min = corners[0];
            Vector2 max = corners[2];
            Vector2 size = max - min;

            return new Rect(min, size);
        }
    }
}
