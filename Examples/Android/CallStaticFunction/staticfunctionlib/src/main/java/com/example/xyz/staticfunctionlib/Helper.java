package com.example.xyz.staticfunctionlib;

import android.content.ContentResolver;
import android.database.Cursor;
import android.graphics.drawable.Drawable;
import android.net.Uri;
import android.provider.MediaStore;
import android.text.TextUtils;
import android.util.Log;

import android.content.Context;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by David on 8/8/2016.
 */

public class Helper {
    /**
     * This function demonstrates the simplet function to be called inside Unity
     */
    public static String DoSthInAndroid() {

		return "return path list of local dcim, and in ugui or ngui of unity, caching 5 slots with scrollview";
        //return "Hi, Sth is done in Android";
    }

    public String[] getPathOfAllImages(ContentResolver contentResolver) {

        List<String> result = new ArrayList<String>();

        result.add("a1");
        result.add("a2");
        result.add("a3");

        Uri uri = android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI;
        String[] projection = { MediaStore.MediaColumns.DATA, MediaStore.MediaColumns.DISPLAY_NAME };

        Cursor cursor = contentResolver.query(uri, projection, null, null, MediaStore.MediaColumns.DATE_ADDED + " desc");
        int columnIndex = cursor.getColumnIndexOrThrow(MediaStore.MediaColumns.DATA);
        int columnDisplayname = cursor.getColumnIndexOrThrow(MediaStore.MediaColumns.DISPLAY_NAME);

        int lastIndex;
        while (cursor.moveToNext())
        {
            String absolutePathOfImage = cursor.getString(columnIndex);
            String nameOfFile = cursor.getString(columnDisplayname);
            lastIndex = absolutePathOfImage.lastIndexOf(nameOfFile);
            lastIndex = lastIndex >= 0 ? lastIndex : nameOfFile.length() - 1;

            if (!TextUtils.isEmpty(absolutePathOfImage))
            {
                result.add(absolutePathOfImage);
            }
        }

        for (String string : result)
        {
            Log.i("getPathOfAllImages", "|" + string + "|");
        }

        String[] Result = result.toArray(new String[0]);

        return  Result;
    }
}

