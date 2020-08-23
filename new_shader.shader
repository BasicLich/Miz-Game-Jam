shader_type canvas_item;

uniform float palette;
uniform sampler2D palette_tex;
void fragment() {

  	vec4 inColour=texture(SCREEN_TEXTURE,SCREEN_UV);
	if(true)
	{
	int colourR=int(inColour.r*255.0);
	int colourG=int(inColour.g*255.0);
	int colourB=int(inColour.b*255.0);
	float x=float(palette);
	vec4 col=vec4(0.8,0.8,0.8,1);
	if(colourR==71 && colourG== 45 && colourB==60)
	{
		col=texture(palette_tex,vec2(0,x));
	}
	else if(colourR==207 && colourG== 198 && colourB==184)
	{
		col.rgb=texture(palette_tex,vec2(1.0/8.0+0.02,x)).rgb;
	}
	else if(colourR==207 && colourG== 198 && colourB==184)
	{
		col.rgb=texture(palette_tex,vec2(2.0/8.0-0.02,x)).rgb;
	}
		else if(colourR==56 && colourG== 217 && colourB==115)
	{
		col.rgb=texture(palette_tex,vec2(3.0/8.0-0.02,x)).rgb;
	}
		else if(colourR==230 && colourG== 72 && colourB==46)
	{
		col.rgb=texture(palette_tex,vec2(4.0/8.0-0.02,x)).rgb;
	}
		else if(colourR==60 && colourG== 172 && colourB==215)
	{
		col.rgb=texture(palette_tex,vec2(5.0/8.0-0.02,x)).rgb;
	}
		else if(colourR==244 && colourG== 180 && colourB==27)
	{
		col.rgb=texture(palette_tex,vec2(6.0/8.0-0.02,x)).rgb;
	}
		else if(colourR==191 && colourG== 121 && colourB==88)
	{
		col.rgb=texture(palette_tex,vec2(7.0/8.0-0.02,x)).rgb;
	}
		else if(colourR==33 && colourG== 21 && colourB==28)
	{
		col.rgb=texture(palette_tex,vec2(8.0/8.0,x)).rgb;
	}
	else if (colourR==43 && colourG== 27 && colourB==36)
	{
		col.rgb=texture(palette_tex,vec2(1,x)).rgb*1.3;
		//col.rgb=texture(palette_tex,vec2(1,x)).rgb*1.3;
	}
	else if (colourR==255 && colourG== 255 && colourB==239)
	{
		col.rgb=texture(palette_tex,vec2(0.125,x)).rgb*1.3;
		//col.rgb=texture(palette_tex,vec2(1,x)).rgb*1.3;
	}
	else
	{
		col.rgb=texture(SCREEN_TEXTURE,SCREEN_UV).rgb;
	}
	COLOR = col;
    }

}